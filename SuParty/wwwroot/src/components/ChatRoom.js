// 引入 Vue 3
const { createApp } = Vue;

// 定義 ChatRoom 組件
const ChatRoom = {
  template: `
<div>
    <h3>聊天室</h3>
    <div v-for="message in messages" :key="message.id">
        {{ message.text }}
    </div>
    <input v-model="newMessage" @keyup.enter="sendMessage" placeholder="輸入訊息..." />
</div>
  `,
  data() {
    return {
      messages: [],
      newMessage: ''
    };
  },
  methods: {
    sendMessage() {
      if (this.newMessage.trim() !== '') {
        this.messages.push({ id: Date.now(), text: this.newMessage });
        this.newMessage = '';
      }
    }
  }
};

// 創建並掛載 Vue 實例
const app = createApp({
  components: {
    ChatRoom
  }
});

app.mount('#app');