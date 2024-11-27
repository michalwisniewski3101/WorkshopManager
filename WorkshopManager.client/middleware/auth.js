export default defineNuxtRouteMiddleware((to, from) => {
  if (process.client) {
    const token = localStorage.getItem('jwtToken');
    if (!token && to.path !== '/auth/login') {
      return navigateTo('/auth/login');
    }
  }
});
