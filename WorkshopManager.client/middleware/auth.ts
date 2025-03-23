export default defineNuxtRouteMiddleware((to, from) => {
  if (process.client) {
    const token = localStorage.getItem('jwtToken');
    const roles = localStorage.getItem('roles') || '[]'; 
    
   
    const parsedRoles = JSON.parse(roles);


    if (!token && to.path !== '/auth/login') {
      return navigateTo('/auth/login');
    }

  
    if (token) {
      const requiredRoles = to.meta?.auth?.roles || []; 

      if (
        requiredRoles.length > 0 && 
        !requiredRoles.some((role) => parsedRoles.includes(role)) 
      ) {
        return navigateTo('/error'); 
      }
    }
  }
});
