import svelte from 'rollup-plugin-svelte';
import resolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';
import livereload from 'rollup-plugin-livereload';
import { terser } from 'rollup-plugin-terser';

const production = !process.env.ROLLUP_WATCH;
const port = process.env.PORT;
const buildDir = '../wwwroot/dist';

export default {
	input: 'src/index.js',
	output: {
		name: 'app',
		format: 'esm',
		sourcemap: true,
		dir: buildDir,
	},
	plugins: [
		svelte({
			// enable run-time checks when not in production
			dev: !production,
			// we'll extract any component CSS out into
			// a separate file — better for performance
			css: css => {
				css.write(`${buildDir}/bundle.css`);
			}
		}),

		// If you have external dependencies installed from
		// npm, you'll most likely need these plugins. In
		// some cases you'll need additional configuration —
		// consult the documentation for details:
		// https://github.com/rollup/rollup-plugin-commonjs
		resolve(),
		commonjs(),
	
		// In dev mode, call `npm run start` once
		// the bundle has been generated
		// !production && serve(),

		// Watch the `public` directory and refresh the
		// browser on changes when not in production
		!production && livereload(
			{
				watch: '../wwwroot',
				port,
			}),
		// If we're building for production (npm run build
		// instead of npm run dev), minify
		production && terser(),
	],

	watch: {
		clearScreen: false
	}
};
