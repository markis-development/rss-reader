RSS Reader
---

1. Git setup:
* `do a fork`
* `git clone git@github.com:<yourlogin>/rss-reader.git`
* `cd rss-reader`
* `git remote rename origin <yourlogin>`
* `git remote add github git@github.com:markis-development/rss-reader.git`
* `git checkout develop`

2. Git workflow:
* `git add .`
* `git commit -m "message"`
* `git push <yourlogin> <yourbranch>`

3. Docker:
* Installation of docker engine: https://docs.docker.com/engine/installation/
* Installation of docker-compose: https://docs.docker.com/compose/install/
* `cp .env.dist .env`
* `docker-compose up`
