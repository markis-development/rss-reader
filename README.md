RSS Reader
---

1. Git setup:
    * `git clone git@github.com:markis-development/rss-reader.git`
    * `cd rss-reader`
    * `git remote rename origin github`
    * `git checkout develop`

2. Git workflow:
    * `git add .`
    * `git commit -m "message"`
    * `git push github <yourbranch>`

3. Docker:
    * Installation of docker engine: https://docs.docker.com/engine/installation/
    * Installation of docker-compose: https://docs.docker.com/compose/install/
    * `cp .env.dist .env`
    * `docker-compose up`
