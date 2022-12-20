<script type="ts">
  // throw new Error("@migration task: Add data prop (https://github.com/sveltejs/kit/discussions/5774#discussioncomment-3292707)");  
	import NavBar from '../../lib/components/NavBar.svelte';
  import Footer from '../../lib/components/Footer.svelte';
  import type { PageData } from './$types';
  
  export let data: PageData;

  // $: {
  //   console.log(data);
  // }
</script>

<NavBar />
<div class="container">
	<main class="pb-3">
    <h1>Weather forecast</h1>
    <p>This component demonstrates fetching data from the server.</p>
    {#if data.forecasts.length}
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
        {#each data.forecasts as item}
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