<template>
  <div class="login-container">
    <van-nav-bar title="登录" class="login-nav">
      <template #left>
        <van-icon name="cross" size="18" @click="onClose" class="nav-icon" />
      </template>
    </van-nav-bar>
    
    <div class="login-form">
      <van-form @submit="loginBtnClick">
        <van-cell-group inset>
          <van-field
            v-model="loginViewData.username"
            name="username"
            label="用户名"
            placeholder="请输入用户名"
            :rules="[{ required: true, message: '请填写用户名' }]"
          />
          <van-field
            v-model="loginViewData.password"
            type="password"
            name="password"
            label="密码"
            placeholder="请输入密码"
            :rules="[{ required: true, message: '请填写密码' }]"
          />
        </van-cell-group>

        <div style="margin: 16px">
          <van-button round block type="primary" native-type="submit">
            登录
          </van-button>
        </div>
      </van-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/store/auth';
import { LoginInputDto } from './apiServices/LoginDto';
import loginApiService from './apiServices/loginApiService';
import { showToast } from 'vant';

const router = useRouter();
const authStore = useAuthStore();

const loginViewData = reactive({
  username: '',
  password: '',
});

const onClose = () => {
  router.back();
};

async function loginBtnClick() {
  try {
    const loginInputDto = new LoginInputDto(loginViewData.username, loginViewData.password);
    await loginApiService.login(loginInputDto);

    if (authStore.isAuthenticated) {
      showToast('登录成功');
      // 获取重定向地址
      const redirect = router.currentRoute.value.query.redirect as string;
      router.push(redirect || '/home');
    }
  } catch (error) {
    showToast({
      message: '登录失败',
      type: 'fail',
    });
    console.error('登录失败:', error);
  }
}
</script>

<style scoped>
.login-container {
  height: 100vh;
  background-color: var(--van-background-2);
}

.login-form {
  padding: 16px;
  margin-top: 20px;
}

:deep(.van-nav-bar) {
  background-color: var(--van-background-2);
}

:deep(.van-nav-bar__title) {
  color: var(--van-text-color);
}

:deep(.van-nav-bar__left) {
  cursor: pointer;
  padding-left: 16px;
}

:deep(.nav-icon) {
  color: var(--van-text-color);
}

@media (prefers-color-scheme: dark) {
  :deep(.nav-icon) {
    color: var(--van-white);
  }

  :deep(.van-nav-bar__title) {
    color: var(--van-white);
  }

  :deep(.van-nav-bar) {
    background-color: var(--van-background-2);
  }

  .login-container {
    background-color: var(--van-black);
  }
}
</style>
