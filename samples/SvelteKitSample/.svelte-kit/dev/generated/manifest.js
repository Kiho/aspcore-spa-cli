const c = [
	() => import("..\\components\\layout.svelte"),
	() => import("..\\components\\error.svelte"),
	() => import("..\\..\\..\\Scripts\\routes\\sample.svelte"),
	() => import("..\\..\\..\\Scripts\\routes\\about.svelte")
];

const d = decodeURIComponent;

export const routes = [
	// Scripts/routes/sample.svelte
	[/^\/sample\/?$/, [c[0], c[2]], [c[1]]],

	// Scripts/routes/about.svelte
	[/^\/about\/?$/, [c[0], c[3]], [c[1]]]
];

// we import the root layout/error components eagerly, so that
// connectivity errors after initialisation don't nuke the app
export const fallback = [c[0](), c[1]()];