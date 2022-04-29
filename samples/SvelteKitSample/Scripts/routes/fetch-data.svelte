<script context="module" lang="ts">
	export const prerender = true;
  export const ssr = true;

  const SERVER_URL = import.meta.env.VITE_SERVER_URL;

  export async function load({ fetch }) {
    const url = `${SERVER_URL}/weatherforecast`;
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
  export let forecasts;
</script>

<section>
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
  
</section>

<style>
	section {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		flex: 1;
	}
</style>