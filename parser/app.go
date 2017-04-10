package main

import (
	"fmt"
	"net/http"
	"os"
	"strings"
	"time"

	"github.com/PuerkitoBio/goquery"
	"github.com/SlyMarbo/rss"
	"github.com/parnurzeal/gorequest"
)

// RSSItem - rss item
type RSSItem struct {
	Category string
	Title    string
	Link     string
	Summary  string
	PubDate  string
}

func main() {
	http.HandleFunc("/", parseHandler)
	http.ListenAndServe(":6578", nil)
}

func parseHandler(w http.ResponseWriter, r *http.Request) {
	go Parse()
	fmt.Fprintln(w, "Parsing started")
}

// Parse - run parsing
func Parse() {
	rssAggregator := os.Getenv("RSS_AGGREGATOR")

	start := time.Now()

	urls := GetLinks(rssAggregator)
	rssItems := GetRSS(urls)

	elapsed := time.Since(start)

	fmt.Printf("Fetched %d rss pages in %s\n", len(rssItems), elapsed)
}

// GetLinks - get all rss links from rssAggregator page
func GetLinks(rssAggregator string) []string {
	urls := []string{}

	doc, err := goquery.NewDocument(rssAggregator)
	if err != nil {
		return urls
	}

	doc.Find(".fl.adres").Each(func(i int, s *goquery.Selection) {
		url, exists := s.Find("a").Attr("href")
		if exists && len(url) > 0 {
			urls = append(urls, url)
			SaveChannel(url)
		}
	})

	return urls
}

// SaveChannel - save channel
func SaveChannel(link string) {
	name := link
	description := ""

	url := fmt.Sprintf("http://reader_api:5000/pushrss?link=%s&name=%s&description=%s", link, name, description)
	gorequest.New().Post(url).End()
}

// SaveRSS - save rss
func SaveRSS(item RSSItem) {
	url := fmt.Sprintf("http://reader_api:500/pushrss?channelLink=%s&link=%s&name=%s&description=%s&pubDate=%s", item.Link, item.Title, item.Summary, item.Category, item.PubDate)
	gorequest.New().Put(url).End()
}

// GetRSS - fetch rss
func GetRSS(urls []string) []RSSItem {
	rssItems := []RSSItem{}

	channelRSSItems := make(chan []RSSItem)

	counter := 0
	for _, url := range urls {
		go getSingleRss(url, channelRSSItems)
		counter++
	}

	for i := 0; i < counter; i++ {
		items := <-channelRSSItems
		for _, item := range items {
			rssItems = append(rssItems, item)
		}
	}

	return rssItems
}

func getSingleRss(url string, channelRSSItems chan []RSSItem) {
	rssItems := []RSSItem{}

	resp, err := http.Get(url)
	if err != nil {
		channelRSSItems <- rssItems
		return
	}
	defer resp.Body.Close()

	contentType := resp.Header.Get("Content-Type")
	if !strings.Contains(contentType, "xml") {
		channelRSSItems <- rssItems
		return
	}

	feed, err := rss.Fetch(url)
	if err != nil {
		channelRSSItems <- rssItems
		return
	}

	for _, item := range feed.Items {
		rssItem := RSSItem{url, item.Title, item.Link, item.Summary, item.Date.Format(time.UnixDate)}
		SaveRSS(rssItem)
		rssItems = append(rssItems, rssItem)
	}

	channelRSSItems <- rssItems
}
