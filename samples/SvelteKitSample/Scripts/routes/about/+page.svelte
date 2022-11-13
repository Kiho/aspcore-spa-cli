<script context="module" lang="ts">
	throw new Error("@migration task: Check code was safely removed (https://github.com/sveltejs/kit/discussions/5774#discussioncomment-3292722)");

	// export const prerender = false; // true;

	// const pageName = 'About'
</script>

<script>
	import NavBar from '../../lib/components/NavBar.svelte';
  import Footer from '../../lib/components/Footer.svelte';
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
