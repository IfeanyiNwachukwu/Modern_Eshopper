FROM nginx

COPY APIGateways/nginx/nginx.local.conf  /etc/nginx/nginx.conf
COPY APIGateways/nginx/id-local.crt /etc/ssl/certs/ifeanyi.eshopping.com.crt
COPY APIGateways/nginx/id-local.key /etc/ssl/private/ifeanyi.eshopping.com.key

