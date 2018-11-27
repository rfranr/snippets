#!/bin/bash
# dummy stres test
x=1
while [ $x -le 200 ]
do
  x=$(( $x + 1 ))
  nohup curl -X POST -H "Content-Type:application/json" -d '{"hello": "world"}' 'https://localhost/api.php?amount=10' 1&> /dev/null < /dev/null
done

