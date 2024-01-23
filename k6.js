import http from 'k6/http';
import { sleep } from 'k6';



export const options = {

      vus: 10,
      duration: '5s'
}


export default function () {

   CreateAddresBook();
   GetAddresses();
   GetWeatherForecast();
}

function GetAddresses() {

   let getParams = {
      url: 'http://localhost:5013/AddressBook',
      payload: {},
      params: {},
      httpVerb: 'get'
   }

   makeRequest(getParams);

}

function GetWeatherForecast() {

   let getParams = {
      url: 'http://localhost:5013/WeatherForecast',
      payload: {},
      params: {},
      httpVerb: 'get'
   }

   makeRequest(getParams);

}


function CreateAddresBook() {

   let postParams = {
      url: 'http://localhost:5013/AddressBook',
      payload: {},
      params: {},
      httpVerb:'post'
   }

   makeRequest(postParams);

}



function makeRequest(requestParams) {

   if (!requestParams && typeof requestParams !== 'object') {
      console.log("Valid post params required")
      return
   }

   if (requestParams.httpVerb == 'get')
      http.get(requestParams.url, requestParams.payload, requestParams.params)
   else
      http.post(requestParams.url, requestParams.payload, requestParams.params)

}

