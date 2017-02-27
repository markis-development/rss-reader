package main

import (
	"fmt"
	"net/http"
	"os"
	"strings"
	"time"

	"github.com/PuerkitoBio/goquery"
	"github.com/SlyMarbo/rss"
)

// RSSItem - rss item
type RSSItem struct {
	Category string
	Title    string
	Link     string
	Summary  string
}

func main() {
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
		}
	})

	return urls
}

// GetRSS - fetch rss
func GetRSS(urls []string) []RSSItem {
	rssItems := []RSSItem{}

	channelRSSItems := make(chan []RSSItem)

	for _, url := range urls {
		go getSingleRss(url, channelRSSItems)
	}

	for _, url := range urls {
		items := <-channelRSSItems
		for _, item := range items {
			rssItem := RSSItem{url, item.Title, item.Link, item.Summary}
			rssItems = append(rssItems, rssItem)
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
		rssItem := RSSItem{url, item.Title, item.Link, item.Summary}
		rssItems = append(rssItems, rssItem)
	}

	channelRSSItems <- rssItems
}
