import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./layout/Layout";
import HomePage from "./pages/HomePage";
import ChatPage from "./pages/ChatPage";
import LoginPage from "./pages/Auth/LoginPage";
import GroupsPage from "./pages/GroupsPage";
import AuthenticatedRoute from "./components/Routing/AuthenticatedRoute";
import { AuthenticationProvider } from "./components/Authentication/AuthenticationProvider";
import GuestLayout from "./layout/GuestLayout";
import RegisterPage from "./pages/Auth/RegisterPage";
import PostsPage from "./pages/PostsPage";
import PostPage from "./pages/PostPage";
import { ToastContainer } from "./components/Toasts/ToastContainer";
import NotificationsManager from "./components/Notifications/NotificationsManager";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import UsersPage from "./pages/UsersPage";

const queryClient = new QueryClient()

export default function App() {
  return (
    <QueryClientProvider client={queryClient}>
    <AuthenticationProvider>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout/>}>
          <Route path="/" element={<AuthenticatedRoute/>}>
            <Route index element={<HomePage/>}/>
            <Route path="chat" element={<ChatPage/>}/>
            <Route path="users" element={<UsersPage/>}/>
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
    
    <NotificationsManager/>
    <ToastContainer/>
    </AuthenticationProvider>
    </QueryClientProvider>
  )
}