/**
 * Gets client IP from 'x-forwarded-for' header, ignoring socket and intermediate proxies.
 * @param {Headers} headers
 * @returns {string} Client IP
 */
 export function getClientIPFromHeaders(headers) {
	/** @type {string} */
	const resHeader = headers.get('x-forwarded-for') ?? '127.0.0.1';
	const [origin] = resHeader.split(', ');
	const [ipAddress] = origin.split(':');

	return ipAddress;
}