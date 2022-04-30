import esbuild from 'esbuild';
import {
  writeFileSync, readFileSync
} from 'fs';
import { join, posix } from 'path';
import { fileURLToPath, URL } from 'url';

// import { Adapter, Builder } from '@sveltejs/kit';

// type BuilderFix = Builder & {
//     utils: {
//         log: {
//             minor: (a:string) => ""
//         },
//         copy: (a:string, b:string) => "",
//         copy_client_files: (a: string) => "",
//         copy_static_files: (a: string) => "",
//     },
//     config: {
//         kit: {
//             appDir: string
//         }
//     }
// } 

// type esBuildOptions = esbuild.BuildOptions;

// type adapterOptions = {
//     out: string,
//     precompress: boolean,
//     env?: {
//         host?: string,
//         port?: string
//     },
//     esbuildOptsFunc?: (defaultOptions: esBuildOptions) => Promise<esBuildOptions>
// }

export default function ({
    out = 'build',
    //precompress = false, //compression will be done in dotnetcore for performance
    esbuildOptsFunc = null,		
		debug = true,
}) {
    const adapter = {
        name: '@sveltejs/adapter-dotnetcore',
        adapt: async (builder) => {
            //utils.update_ignores({ patterns: [out] });
            builder.log.minor('Copying assets')
            const static_directory = join(out, 'assets')

            builder.writeStatic(static_directory);
            builder.writeClient(static_directory);

            builder.log.minor('Building server');
            //const files = fileURLToPath(new URL('./files', import.meta.url));
            const files = fileURLToPath(new URL('./', import.meta.url));
            builder.copy(files, '.svelte-kit/dotnetcore');

            const tmp = '.svelte-kit/dotnetcore';
            const entry = `${tmp}/entry.js`;
            let relativePath = '../output/server'; // posix.relative(tmp, builder.getServerDirectory());

            console.log('relativePath: ', relativePath);

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
                //entryPoints: ['.svelte-kit/node/index.js'],
                // entryPoints: ['.svelte-kit/dotnetcore/entry.js'],
                entryPoints: [entry],
                outfile: join(out, 'index.cjs'),
                bundle: true,
                external: Object.keys(JSON.parse(readFileSync('package.json', 'utf8')).dependencies || {}),
                format: 'cjs',
                platform: 'node',
                target: 'node12',
                // inject: [join(files, 'shims.js')],
                define: {
                    //esbuild_app_dir: '"' + config.kit.appDir + '"'
                    esbuild_app_dir: '"' + builder.config.kit.appDir + '"'
                }
            };
            const buildOptions = esbuildOptsFunc ? await esbuildOptsFunc(defaultOptions) : defaultOptions;
            await esbuild.build(buildOptions);

            // TBD - Add prerender here; prerendering requires a live dotnetcore 
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