<script>
  import { getStore } from '../stores.js';
  let storeCount = getStore('count', 0);
  let internalCount = 0;
  function incrementCounts() {
    internalCount++;
    $storeCount++;
  }
</script>

<style>
  .counters {
    display: inline-block;
    padding: 1rem;
    margin: 1rem;
    border: 1px solid orangered;
    border-radius: 0.25rem;
  }
  .counters > * {
    margin: 0 0.5rem;
  }
</style>

<h2>Counter</h2>
<p>This counter component stores state in 2 different ways to demonstrate the effects of hmr updates.</p>
<p>
  TL;DR: internal state gets recreated if a component or parent is recreated. state in an external store is preserved as long as the store
  itself is not updated or the store uses extra code to preserve its state
</p>

<span class="counters">
  <span>interal: {internalCount}</span>
  <span>store: {$storeCount}</span>
  <button on:click={incrementCounts}>+</button>
</span>
