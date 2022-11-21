import { sveltekit } from '@sveltejs/kit/vite';
// import { searchForWorkspaceRoot } from 'vite';

const config = {
	plugins: [sveltekit()],

  clearScreen: false,

  resolve: {
    dedupe: ['svelte'],
  },
};

export default config;
