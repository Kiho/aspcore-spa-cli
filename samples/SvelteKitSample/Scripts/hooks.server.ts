export function handleError({ error }) {  
  console.log('handleError: ', error);
  // console.log('event: ', event);
  return {
    message: error,
    code: error.code ?? 'UNKNOWN'
  };
}