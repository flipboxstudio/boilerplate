import { ApplicationUserState } from './../../classes/auth/ApplicationUserState'
import { AuthState } from './../../classes/auth/AuthState'
import { SET_USER } from './mutation-types'

export default {
  actions: {},
  getters: {},
  mutations: {
    [SET_USER] (state: AuthState, user: ApplicationUserState) {
      state.user = user
    }
  },
  namespaced: true,
  state: new AuthState()
}
