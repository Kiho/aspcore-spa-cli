import esbuild from 'esbuild';
import {
  writeFileSync, readFileSync
} from 'fs';
import { join } from 'path';
import { fileURLToPath, URL } from 'url';

export default function ({
  out = 'build',
  //precompress = false, //compression will be done in aspcore for performance
  esbuildOptsFunc = null,		
  debug = true,
}) {
  const adapter = {
    name: '@sveltejs/adapter-aspcore',
    adapt: async (builder) => {
      const tmp = '.svelte-kit/aspcore';
      const entry = `${tmp}/entry.js`;
      const staticDirectory = join(out, 'assets');

      builder.rimraf(tmp);
      builder.rimraf(staticDirectory);

      builder.log.minor('Building server');
      // const files = fileURLToPath(new URL('./files', import.meta.url));
      const files = fileURLToPath(new URL('./', import.meta.url));
      builder.copy(files, tmp);

      let relativePath = '../output/server';
      builder.rimraf(relativePath);

      builder.copy(join(files, 'entry.js'), entry, {
        replace: {
          SERVER: `${relativePath}/index.js`,
          MANIFEST: './manifest.js',
          DEBUG: debug.toString()
        }
      });

      writeFileSync(
        `${tmp}/manifest.js`,
        `export const manifest = ${builder.generateManifest({
          relativePath
        })};\n`
      );
      
      const defaultOptions = {
        entryPoints: [entry],
        outfile: join(out, 'index.cjs'),
        bundle: true,
        external: Object.keys(JSON.parse(readFileSync('package.json', 'utf8')).dependencies || {}),
        format: 'cjs',
        platform: 'node',
        target: 'node12',
      };
      
      const buildOptions = esbuildOptsFunc ? await esbuildOptsFunc(defaultOptions) : defaultOptions;
      await esbuild.build(buildOptions);

      builder.log.minor('Copying assets');
      builder.writeStatic(staticDirectory);
      builder.writeClient(staticDirectory);
      builder.writePrerendered(staticDirectory);

      // TBD - Add prerender here; prerendering requires a live aspcore 
      //       server, need to put a bit of thought how it should be setup
      //
      //utils.log.minor('Prerendering static pages');
      // await utils.prerender({
      //     dest: `${out}/prerendered`
      // });
      // if (precompress && existsSync(`${out}/prerendered`)) {
      //     utils.log.minor('Compressing prerendered pages');
      //     await compress(`${out}/prerendered`);
      // }   await compress(`${out}/prerendered`);
      // }
    }
  };

  return adapter;
}