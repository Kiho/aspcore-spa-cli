<script context="module" lang="ts">
	export const prerender = true;
  export const ssr = true;
  
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

    return {
      status: response.status,
      props: {
        forecasts: data
      }
    };
  }
</script>

<script>  
	import NavBar from '../lib/components/NavBar.svelte';
  import Footer from '../lib/components/Footer.svelte';
  
  export let forecasts;
</script>

<NavBar />
<div class="container">
	<main class="pb-3">
    <h1>Weather forecast</h1>
    <p>This component demonstrates fetching data from the server.</p>
  
    {#if forecasts.length}
    <table class="table">
      <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
      </thead>
      <tbody>
        {#each forecasts as item}
        <tr>
          <td>{ item.dateFormatted }</td>
          <td>{ item.temperatureC }</td>
          <td>{ item.temperatureF }</td>
          <td>{ item.summary }</td>
        </tr>
        {/each}
      </tbody>
    </table>
    {:else}
    <p><em>Loading...</em></p>
    {/if}
	</main>
</div>
<Footer />