#!/usr/bin/env python
# coding: utf-8

# In[45]:


file = open("hospital_visits.csv", "r")

max_value = 0

list_FIPS =[]
List_values=[]

c = 0
for line in file:
    if(c == 0):
        c+=1
    else:
        line = line.split(",")

        for i in range(1,len(line)):
            value = float(line[i])
            max_value = ((value > max_value) * value) + ((value <= max_value) * max_value)

        list_FIPS.append(line[0])
        List_values.append(line[1:])
        c+=1
        
file.close()


# In[46]:


list_values_normalized = []
for row in List_values:
    listRow = []
    for value in row:
        listRow.append(float(value) / max_value)
    list_values_normalized.append(listRow)


# In[47]:


file = open("hospital_visits_Normalized.csv", "w")

for i in range(len(list_values_normalized)):
    FIPS = list_FIPS[i]
    file.write(FIPS + ',')
    
    row = list_values_normalized[i]
    for value in row:
        file.write(str(value) + ',')
    file.write("\n")
file.close()


# In[ ]:




