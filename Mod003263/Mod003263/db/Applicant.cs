﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod003263.DBstuff {
    public class Applicant {
        private int m_id;
        public int ID { get { return m_id; } set { m_id = value; } }

        private String m_First_Name;
        public String First_Name { get { return m_First_Name; } set { m_First_Name = value; } }

        private String m_Last_Name;
        public String Last_Name { get { return m_Last_Name; } set { m_Last_Name = value; } }

        private String m_Applying_Position;
        public String Applying_Position { get { return m_Applying_Position; } set { m_Applying_Position = value; } }

        private String m_Picture;
        public String Picture { get { return m_Picture; } set { m_Picture = value; } }

        private String m_address;
        public String Address { get { return m_address; } set { m_address = value; } }

        private String m_Phone_Number;
        public String Phone_Number { get { return m_Phone_Number; } set { m_Phone_Number = value; } }

        private String m_email;
        public String Email { get { return m_email; } set { m_email = value; } }

        private long m_Dob;
        public long Dob { get { return m_Dob; } set { m_Dob = value; } }

        public String Full_Name => First_Name + " " + Last_Name;
    }
}