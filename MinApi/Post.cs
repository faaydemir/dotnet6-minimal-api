namespace Post;
public class PostEntry
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
}

public class PostStore
{
    private List<PostEntry> _posts;
    public PostStore()
    {
        _posts = new List<PostEntry>();
    }
    public List<PostEntry> All()
    {
        return _posts;
    }
    public PostEntry Get(int id)
    {
        return _posts.FirstOrDefault(p => p.Id == id);
    }
    public bool Any(int id)
    {
        return _posts.Any(p => p.Id == id);
    }
    public void Add(PostEntry post)
    {
        post.Created = DateTime.UtcNow;
        _posts.Add(post);
    }
    public void Update(PostEntry post)
    {
        var po = _posts.FirstOrDefault(p => p.Id == post.Id);
        po.Title = post.Title;
        po.Content = post.Content;
    }
    public void Delete(int id)
    {
        _posts.RemoveAll(r => r.Id == id);
    }
}