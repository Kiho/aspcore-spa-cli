import type { PageLoad } from './$types';

export const prerender = false;
export const ssr = true;

const SERVER_URL = import.meta.env.VITE_SERVER_URL;

function getServerUrl() {
  if (typeof window !== 'undefined') {
    return '';
  }
  return process.env.ASPNETCORE_URLS || SERVER_URL;
}

export const load: PageLoad = async () =>  {
  const serverUrl = getServerUrl();
  const url = `${serverUrl}/weatherforecast`;
  console.log('fetch ', url);
  const response = await fetch(url);

  let data: any;
  const contentType: string = response.headers.get('content-type');
  console.log('content-type', contentType);
  if (contentType && contentType.indexOf('application/json') !== -1) {
    data = response.json();
  } else {
    data = response.text();
  }
  
  return { forecasts: data, serverUrl: url, contentType };
}
