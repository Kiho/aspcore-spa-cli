<script context="module" lang="ts">
	const pageName = 'About'
</script>

<script>
	import NavBar from '../lib/components/NavBar.svelte';
  import Footer from '../lib/components/Footer.svelte';
</script>

<svelte:head>
	<title>Sveltekit Page - {pageName}</title>
</svelte:head>

<NavBar />
<div class="container">
	<main class="pb-3">
		<h1>Sveltekit + dotnet - {pageName} page</h1>
	</main>
</div>
<Footer />
