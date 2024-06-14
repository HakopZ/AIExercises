namespace Visualizer
{



    public partial class Form1 : Form
    {
        Graphics gfx;
        Graph<State> graph = new Graph<State>();
        Queue<Vertex<State>> queue = [];
        Bitmap bitmap;

        public Form1()
        {
            InitializeComponent();
        }

        private void AddNode_Click(object sender, EventArgs e)
        {
            State state = new State();
            Vertex<State> v = new Vertex<State>(state);
            var split = textBoxLocation.Text.Split(',');
            var location = new Point(int.Parse(split[0]), int.Parse(split[1]));
            v.Location = location;
            graph.AddVertex(v);
            gfx.FillEllipse(Brushes.Gray, v.Hitbox);
            pictureBox.Image = bitmap;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            gfx = Graphics.FromImage(bitmap);




        }

        private void AddEdgeButton_Click(object sender, EventArgs e)
        {
            if (queue.Count != 2) return;
            var t1 = queue.Dequeue();
            var t2 = queue.Dequeue();
            graph.AddEdge(t1, t2, int.TryParse(weightTextbox.Text, out int result) ? result : 0);
            gfx.DrawLine(Pens.Green, t1.Location + (t1.Hitbox.Size / 2), t2.Location + (t2.Hitbox.Size / 2));
            Label label = new Label()
            {
                Location = new Point((t1.Location.X + t2.Location.X) / 2 + t1.Hitbox.Width, (t1.Location.Y + t2.Location.Y) + t1.Hitbox.Height / 2),
                Text = result.ToString()
            };
            Controls.Add(label);
            pictureBox.Image = bitmap;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            var mE = (MouseEventArgs)e;
            foreach(var v in graph.Vertices)
            {
                if (v.Hitbox.IntersectsWith(new Rectangle(mE.Location, new Size(1, 1))))
                {
                    if(queue.Count == 2)
                    {
                        var t = queue.Dequeue();
                        gfx.FillEllipse(Brushes.Gray, t.Hitbox);
                    }
                    gfx.FillEllipse(Brushes.Red, v.Hitbox);
                    queue.Enqueue(v);
                }
            }
            pictureBox.Image = bitmap;
        }
    }
}
