import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./layout/Layout";
import HomePage from "./pages/HomePage";
import ChatPage from "./pages/ChatPage";
import LoginPage from "./pages/Users/LoginPage";
import GroupsPage from "./pages/GroupsPage";
import AuthenticatedRoute from "./components/Routing/AuthenticatedRoute";
import { AuthenticationProvider } from "./components/Authentication/AuthenticationProvider";
import GuestLayout from "./layout/GuestLayout";
import RegisterPage from "./pages/Users/RegisterPage";
import PostsPage from "./pages/PostsPage";
import PostPage from "./pages/PostPage";
import { NotificationProvider } from "./notifications/NotificationProvider";
import { ToastContainer } from "./components/Toasts/ToastContainer";

export default function App() {
  return (
    <NotificationProvider>
    <AuthenticationProvider>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout/>}>
          <Route path="/" element={<AuthenticatedRoute/>}>
            <Route index element={<HomePage/>}/>
            <Route path="chat" element={<ChatPage/>}/>
            <Route path="groups" element={<GroupsPage/>}/>
            <Route path="posts" element={<PostsPage/>}/>
            <Route path="posts/:slug" element={<PostPage/>}/>
          </Route>
        </Route>
        <Route path="/" element={<GuestLayout/>}>
          <Route path="login" element={<LoginPage/>}/>
          <Route path="register" element={<RegisterPage/>}/>
        </Route>
      </Routes>
    </BrowserRouter>

    <ToastContainer/>
    </AuthenticationProvider>
    </NotificationProvider>
  )
}