export const prerender = false; // true;
const SERVER_URL = import.meta.env.VITE_SERVER_URL;
function getServerUrl() {
    if (typeof window !== 'undefined') {
        return '';
    }
    return process.env.ASPNETCORE_URLS || SERVER_URL;
}
export async function load({ fetch }) {
    const serverUrl = getServerUrl();
    console.log('serverUrl', serverUrl);
    const url = `${serverUrl}/weatherforecast`;
    const response = await fetch(url);
    const data = await response.json();
    throw new Error("@migration task: Migrate this return statement (https://github.com/sveltejs/kit/discussions/5774#discussioncomment-3292693)");
    return {
        status: response.status,
        props: {
            forecasts: data
        }
    };
}
//# sourceMappingURL=+page.js.map