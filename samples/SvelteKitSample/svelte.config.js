import preprocess from 'svelte-preprocess';
import adapter from 'svelte-adapter-aspcore';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	preprocess: preprocess(),
	kit: {		
    files: {
			assets: 'Assets',
			hooks: 'Scripts/hooks',
			lib: 'Scripts/lib',
			routes: 'Scripts/routes',
			serviceWorker: 'Scripts/service-worker',
			template: 'Scripts/app.html'
		},
		adapter: adapter({ out : 'build' }),
	},
	compilerOptions: {
		hydratable: true,
		customElement: false,
	}
};

export default config;
