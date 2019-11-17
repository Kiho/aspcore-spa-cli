<script>
    import { onMount } from 'svelte';

    export let forecasts = [];

    onMount(() => {
        fetch('/api/weatherforecast')
            .then(response => response.json())
            .then(data => {
                forecasts = data;
            });
    });
</script>

<div>
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
    
</div>
