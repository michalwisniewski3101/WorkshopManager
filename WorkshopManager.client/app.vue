<template>
  <div ><NuxtLayout> </NuxtLayout></div>
</template>

<style>
  v-btn {
    margin-top: 1rem;
    transition: background 0.3s ease;
  }
  
  h1 {
    color: #000000;
    margin-bottom: 1rem;
    font-family: 'Arial', sans-serif;
  }
  
  v-card {
    padding: 20px;
    background: #e0e0e0;
    border-radius: 10px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1) !important;
  }
  
  v-text-field {
    max-width: 300px;
    margin-left: auto;
    color: #000;
  }
  
  .clickable-icon {
    color: #000000;
    transition: color 0.3s ease;
  }
  
  .clickable-icon:hover {
    color: #45a049;
  }
  
  :deep(.v-data-table) {
    background: rgb(185, 185, 185);
    border-radius: 5px;
    margin-top: 1rem;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05) !important;
  }
  
  :deep(.v-data-table th) {
    font-weight: bold;
    color: #000000;
    background: #a0a0a0;
  }
  
  :deep(.v-data-table td) {
    color: #000000;
  }
  .v-breadcrumbs:deep() {
    color: #000000
  }
</style>