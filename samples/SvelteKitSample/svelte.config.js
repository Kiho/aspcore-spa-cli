// @ts-ignore
// import adapter from 'sveltekit-adapter-dotnetcore';
import preprocess from 'svelte-preprocess';
// import adapter from '@sveltejs/adapter-node';
import adapter from './adapter/index.js';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	// Consult https://github.com/sveltejs/svelte-preprocess
	// for more information about preprocessors
	preprocess: preprocess(),
	// onwarn: (warning, handler) => {
	// 	console.warn('warning', warning);
	// 	handler(warning);
	// },
	kit: {
		// hydrate the <div id="svelte"> element in src/app.html	
    files: {
			assets: 'Assets',
			hooks: {
        client: 'Scripts/hooks.client',
        server: 'Scripts/hooks'
      },
			lib: 'Scripts/lib',
			routes: 'Scripts/routes',
			serviceWorker: 'Scripts/service-worker',
			appTemplate: 'Scripts/app.html'
		},
		adapter: adapter({ out : 'build', precompress : false})
	},
	compilerOptions: {
		hydratable: true,
		customElement: false,
		// dev: true
	}
};

export default config;
