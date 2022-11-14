export const prerender = false; // true;

const SERVER_URL = import.meta.env.VITE_SERVER_URL;

function getServerUrl() {
  if (typeof window !== 'undefined') {
    return '';
  }
  return process.env.ASPNETCORE_URLS || SERVER_URL;
}

export async function load({ fetch }) {
  const serverUrl = getServerUrl();
  console.log('serverUrl', serverUrl);
  const url = `${serverUrl}/weatherforecast`;
  const response = await fetch(url);
  const data = await response.json();

  // const data = [{ dateFormatted: 'dateFormatted', summary: 'summary' }];
  // const body = JSON.stringify({ forecasts });
  return { forecasts: data, serverUrl };
}
