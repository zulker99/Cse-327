using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AcademicAssistant.Models
{
    /// <summary>
    /// Represents a comment made by a user on a post.
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the comment.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who made the comment.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the post on which the comment was made.
        /// </summary>
        public string PostID { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the status of the comment.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the comment was made.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who made the comment.
        /// </summary>
        public string UserName { get; set; }
    }

    /// <summary>
    /// Represents a post made by a user.
    /// </summary>
    public class _ShowPost
    {
        /// <summary>
        /// Gets or sets the unique identifier for the post.
        /// </summary>
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the post.
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image associated with the post.
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the post was created.
        /// </summary>
        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who created the post.
        /// </summary>
        [Required]
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who created the post.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the status of the post.
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the list of comments associated with the post.
        /// </summary>
        public List<CommentViewModel> Comments { get; set; }
    }
}
