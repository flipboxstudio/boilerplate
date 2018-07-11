<template>
  <div>
    <nav
    class="navbar navbar-dark sticky-top bg-dark flex-md-nowrap p-0">
      <a
        class="navbar-brand col-sm-3 col-md-2 mr-0"
        href="/">
        App
      </a>
      <ul
      class="navbar-nav px-3">
        <li
        class="nav-item text-nowrap">
          <a
            class="nav-link"
            href="/Auth/Logout">
            Sign out as {{ authUser.email }}
          </a>
        </li>
      </ul>
    </nav>

    <div
    class="container-fluid">
      <div
      class="row">
        <nav
        class="col-md-2 d-none d-md-block bg-light sidebar">
          <div
          class="sidebar-sticky">
            <ul
            class="nav flex-column">
              <li
              class="nav-item">
                <router-link
                  to="/"
                  :exact="true"
                  class="nav-link active">
                  Home
                </router-link>
              </li>

              <li
              class="nav-item">
                <router-link
                  to="/about"
                  :exact="true"
                  class="nav-link active">
                  About
                </router-link>
              </li>
            </ul>
          </div>
        </nav>

        <main
          role="main"
          class="col-md-9 ml-sm-auto col-lg-10 pt-3 px-4">
          <router-view/>
        </main>
      </div>
    </div>
  </div>
</template>

<script lang="ts" src="./App.ts"></script>
