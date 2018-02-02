<template>
    <div>
        <h1>Hello from Vue!</h1>

        <router-link to="/" :exact="true">Home</router-link>
        <router-link to="/about" :exact="true">About</router-link>

        <router-view></router-view>
    </div>
</template>