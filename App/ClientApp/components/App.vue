<template>
  <div>
    <h1>App</h1>

    <router-link
      to="/"
      :exact="true">Home</router-link>
    <router-link
      to="/about"
      :exact="true">About</router-link>
    <a href="/Auth/Logout">Logout</a>

    <router-view/>
  </div>
</template>

<script src="./App.ts"></script>
