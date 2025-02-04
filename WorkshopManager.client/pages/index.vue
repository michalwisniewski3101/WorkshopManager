<template>
    <v-card style="height: 100%; width: 100%;">
    <div>
        <h1>Witamy w aplikacji WorkshopManager</h1>
        <p>Ta aplikacja pozwala na zarządzanie warsztatem samochodowym.</p>
    </div>
    </v-card>
</template>

<script setup>
definePageMeta({
  middleware: 'auth',
  auth: {
    roles: ["Administrator", "Starszy Mechanik", "Młodszy Mechanik", "Klient"]
  },
  
    layout: 'default'
  
})
</script>

<style scoped>

a {
    color: #007bff;
    text-decoration: none;
}

a:hover {
    text-decoration: underline;
}
</style>