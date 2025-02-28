import { useParams } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import WaitForQuery from "../components/WaitForQuery";
import Post from "../features/Posts/Post";
import { getPostBySlug } from "../endpoints/postEndpoints";

export default function ReadPostPage() {
    const { slug } = useParams()

    if (!slug) {
        throw new Error('The PostPage requires a URL parameter by the name "slug"')
    }

    const query = useQuery({
        queryKey: ['post-slug', slug],
        queryFn: () => getPostBySlug(slug)})

    return (
        <WaitForQuery query={query}>
            <Post post={query.data!}/>
        </WaitForQuery>
    )
}