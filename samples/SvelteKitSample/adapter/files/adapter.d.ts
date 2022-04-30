import { Adapter } from '@sveltejs/kit';
// import { CustomStaticWebAppConfig } from './types/swa';

declare function plugin(opts?: {
  out?: string,
	debug?: boolean;
	// customStaticWebAppConfig?: CustomStaticWebAppConfig;
}): Adapter;
export = plugin;