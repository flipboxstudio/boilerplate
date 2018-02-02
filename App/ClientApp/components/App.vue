<template>
    <div>
        <h1>Hello from Vue!</h1>

        <router-link to="/" :exact="true">Home</router-link>
        <router-link to="/about" :exact="true">About</router-link>

        <router-view></router-view>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
    mounted() {
        console.log('App mounted');
    }
});
</script>
