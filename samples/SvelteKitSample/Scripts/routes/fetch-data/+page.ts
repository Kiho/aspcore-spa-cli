export const prerender = false;
export const ssr = false;

const SERVER_URL = import.meta.env.VITE_SERVER_URL;

function getServerUrl() {
  if (typeof window !== 'undefined') {
    return '';
  }
  return process.env.ASPNETCORE_URLS || SERVER_URL;
}

export async function load({ fetch }) {
  const serverUrl = getServerUrl();
  const url = `${serverUrl}/weatherforecast`;
  console.log('url', url);
  const response = await fetch(url);

  let data;
  const contentType = response.headers.get('content-type');
  console.log('content-type', contentType);
  if (contentType && contentType.indexOf('application/json') !== -1) {
    data = response.json();
  } else {
    data = response.text();
  }
  // const data = [{ dateFormatted: 'dateFormatted', summary: 'summary' }];
  // const body = JSON.stringify({ forecasts });
  return { forecasts: data, serverUrl: url, contentType };
}
