import adapter from 'sveltekit-adapter-dotnetcore';
import preprocess from 'svelte-preprocess';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	// Consult https://github.com/sveltejs/svelte-preprocess
	// for more information about preprocessors
	preprocess: preprocess(),
	onwarn: (warning, handler) => {
		console.warn('warning', warning);
		handler(warning);
	},
	kit: {
		// hydrate the <div id="svelte"> element in src/app.html
		target: '#svelte',
        files: {
			assets: 'Assets',
			hooks: 'Scripts/hooks',
			lib: 'Scripts/lib',
			routes: 'Scripts/routes',
			serviceWorker: 'Scripts/service-worker',
			template: 'Scripts/app.html'
		},
		adapter: adapter({ out : 'build', precompress : false})
	},
	compilerOptions: {
		hydratable: true,
		customElement: false,
		dev: true
	}
};

export default config;
