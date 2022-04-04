#!/usr/bin/env python
# coding: utf-8

# In[9]:


import pandas as pd

df = pd.read_csv("1976-2016-president.csv")


# In[16]:


x = df[["year","state","party","candidatevotes","totalvotes"]]
x


# In[11]:


df.to_csv('trimedData.csv', index = False) 

