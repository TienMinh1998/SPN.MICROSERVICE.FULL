﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCore.Domain.SeedWork;
using DatabaseCore.Domain.Events;

namespace DatabaseCore.Domain.Questions
{
    public class QuestionStandard : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pk_QuestionStandard_Id { get; set; }
        public string English { get; set; }          // Từ bằng tiếng anh
        public string Phonetic { get; set; }         // Phiên tâm
        public string MeaningEnglish { get; set; }   // Nghĩa tiếng anh
        public string MeaningVietNam { get; set; }   // Nghĩa tiếng việt
        public string Note { get; set; }             // Ghi chú
        public bool IsDeleted { get; set; }          // đã xóa hay chưa
        public bool Added { get; set; }              // đã add vào topic hay chưa
        public DateTime created_on { get; set; }     // THời gian tạo
        public string Audio { get; set; }
        public int UserId { get; set; }

        public void AddNote(string noteForQuestion)
        {
            if (!string.IsNullOrEmpty(noteForQuestion))
            {
                Note = Note + ", " + noteForQuestion;
                AddDomainEvent(new ChangeNoteQuestionStanDomainEvent(Id, noteForQuestion));
            }
        }
    }
}
