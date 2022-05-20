FROM node:12.19.0-alpine3.9

WORKDIR /usr/src/api

COPY package*.json ./

RUN npm install glob rimraf

RUN npm install

COPY . .

RUN npm run build

CMD npm run start:debug