git switch master

git pull

docker build . -t coc_stats_tracker:1.0.0
docker rm $(docker stop $(docker ps -aqf "name=coc_stats_tracker")) || true
docker run -d -it --name coc_stats_tracker coc_stats_tracker:1.0.0
