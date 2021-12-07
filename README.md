# SouthernX
Evaluation test

## Demo
- [Frontend](http://southernx.azurewebsites.net)
- [Api](https://southernx-api.azurewebsites.net) (I haven't installed swagger, so you wont see anything until you hit endpoint)

## Backend Architecture
Backend Architecture is designed to support Microservice Architecture, It is a loose and practical form of Clean Architecture. Purely developed on ASP.Net core 3.1. This Implementation also contains Simple Memory Caching for backend data.
Since application had a few requirements, I didn't feel breaking backend in more than 2 projects. Backend also contains some basic level Integration unit tests but you need to update mongo connection to make it work (mocks are not present).

## Frontend Architecture
Frontend Architecture is designed on React and Redux in Typescript using CRA [Create React App](https://github.com/facebookincubator/create-react-app).
I made few changes in UI, since Service Date was not provided in a Mock_data.json I have converted that into Date of Birth search. you could either search by DOB or by Policy Number. if you using DOB. Policy Number wont show validation error. but if you keep both fields empty it will show validation error on policy number.

## MongoDb
For the database engine I have used mongo, because it's easy to use with json data. 

## DevOps
For easy setup all projects can be run through `docker-compose`. API also exposed to use out side of docker too.

## Run
Project can be Run in 2 ways.
- By running instances of `SouthernCross.WebApi`, and `SouthernCross.React` projects through VS (for React you can also use `npm start` command through CLI)
- By running `docker-compose build && docker-compose up`

