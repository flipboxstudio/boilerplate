import Vue from 'vue'
import { createNamespacedHelpers } from 'vuex'
import { IAuth } from '../typing'
const { mapState } = createNamespacedHelpers('auth')

export default Vue.extend({
  computed: {
    ...mapState({
      authUser: (state: IAuth) => state.user
    })
  },
  mounted () {
    this.$log('App mounted')
  }
})
