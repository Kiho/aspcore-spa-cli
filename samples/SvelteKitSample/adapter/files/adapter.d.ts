import { Adapter } from '@sveltejs/kit';
import esbuild from 'esbuild';

type esBuildOptions = esbuild.BuildOptions;

declare function plugin(opts?: {
  out?: string,
	debug?: boolean;
	esbuildOptsFunc?: (defaultOptions: esBuildOptions) => Promise<esBuildOptions>;
}): Adapter;

export = plugin;